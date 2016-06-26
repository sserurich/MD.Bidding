angular
    .module('homer')
    .controller('ClientEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal) {
        var clientId = $scope.clientId;
        var action = $scope.action;

        if (action == 'create') {
            clientId = 0;
        }

        if (action == 'edit') {
            var promise = $http.get('/webapi/ClientApi/GetClient?clientId=' + clientId, {});
            promise.then(
                function (payload) {
                    var m = payload.data;

                    $scope.client = {
                        ClientId: m.ClientId,
                        FirstName: m.FirstName,
                        LastName: m.LastName,
                        Email: m.Email,                       
                        TimeStamp: m.TimeStamp,
                        CreatedOn: m.CreatedOn,
                        CreatedBy: m.CreatedBy,
                        UpdatedBy: m.UpdatedBy,
                    };
                });
        }


        $scope.Save = function (client) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/ClientApi/SaveClient', {
                    ClientId: clientId,
                    FirstName: client.FirstName,
                    LastName: client.LastName,
                    Email: client.Email                    
                });

                promise.then(
                    function (payload) {
                        ClientId = payload.data;
                        $scope.showMessageSave = true;
                        $timeout(function () {
                            $scope.showMessageSave = false;
                        }, 2000);
                    });
            }
        }

        $scope.Cancel = function () {
            $location.url('/Clients');
        };

        $scope.Delete = function (clientId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/ClientApi/DeleteClient?clientId=' + clientId, {});
            promise.then(
                function (payload) {
                    $scope.showMessageDeleted = true;
                    $timeout(function () {
                        $scope.showMessageDeleted = false;
                    }, 2000);
                    $scope.showMessageDeleteFailed = false;
                },
                function (errorPayload) {
                    $scope.showMessageDeleteFailed = true;
                });
        }
       
    }
    ]);


angular
    .module('homer').controller('ClientController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils',
        function ($scope, ngTableParams, $http, $filter, $location, Utils) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/ClientApi/GetAllClients');
            promise.then(
                function (payload) {
                    $scope.gridData.data = payload.data;
                    $scope.loadingSpinner = false;
                }
            );

            $scope.gridData = {
                enableFiltering: true,
                columnDefs: $scope.columns,
                enableRowSelection: true
            };

            $scope.gridData.multiSelect = true;

            $scope.gridData.columnDefs = [
                { name: 'ClientId', field: 'ClientId', width: '10%', },
                { name: 'FirstName', width: '15%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/clients/edit/{{row.entity.ClientId}}">{{row.entity.FirstName}}</a> <ul class="dropdown-menu dropdown-menu-right"><li><a href="href="#/clients/edit/{{row.entity.ClientId}}">Add New Order</a></li></ul></div>' },
                { name: 'LastName', field: 'LastName', width: '15%' },
                { name: 'Email', field: 'Email', width: '25%' },
                { name: 'Updated On', field: 'TimeStamp', width: '15%', cellFilter: 'date:"yyyy-MM-dd HH:mm:ss"' },
                { name: 'Action', width: '20%',cellTemplate: '<div class="ui-grid-cell-contents"> <a href="#/orders/create/{{row.entity.ClientId}}/0">Add New Order</a></div>' },
            ];

            $scope.gridData.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
            };

            $scope.Delete = function (client) {

                var items = $scope.gridApi.selection.getSelectedRows($scope.gridData);
                $scope.showMessageDelete = false;

                $scope.selectedIds = [];
                items.forEach(function (item) {
                    $scope.selectedIds.push(item.ClientId);
                });

                var promise = $http.post('/webapi/ClientApi/DeleteMultipleClients', {
                    clientIds: $scope.selectedIds,
                });
                promise.then(
                    function (payload) {
                        $scope.showMessageDelete = true;

                        items.forEach(function (item) {
                            var index = $scope.gridData.data.indexOf(item)
                            $scope.gridData.data.splice(index, 1);
                        });

                        $timeout(function () {
                            $scope.showMessageDelete = false;
                        }, 2000);
                        
                        $scope.showMessageDeleteFailed = false;
                    },

                    function (errorPayload) {
                        $scope.showMessageDeleteFailed = true;
                    }
                );
            }


        }]);
