angular
    .module('homer')
    .controller('OrderEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal) {
        var orderId = $scope.orderId;
        var action = $scope.action;
        var clientId = $scope.clientId;

        if (action == 'create') {
            orderId = 0;
        }

        if (action == 'edit') {
            var promise = $http.get('/webapi/OrderApi/GetOrder?orderId=' + orderId, {});
            promise.then(
                function (payload) {
                    var o = payload.data;

                    $scope.order = {
                        OrderId: o.OrderId,
                        ClientId : o.ClientId,
                        PackageTypeId: o.PackageTypeId,
                        StartOn :o.StartOn != null ? moment(o.StartOn).format('YYYY-MM-DD HH:mm:ss') : null,
                        Duration : o.Duration,
                        MealsDaysPerWeek : o.MealsDaysPerWeek,
                        NoOfMealsPerDay : o.NoOfMealsPerDay,                        
                        TimeStamp: o.TimeStamp,
                        CreatedOn: o.CreatedOn,
                        CreatedBy: o.CreatedBy,
                        UpdatedBy: o.UpdatedBy,
                    };
                });
        }


        $scope.Save = function (order) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {                
                var promise = $http.post('/webapi/OrderApi/SaveOrder', {
                    OrderId: order.OrderId,
                    ClientId: clientId,
                    PackageTypeId: order.PackageTypeId,
                    StartOn: order.StartOn,
                    Duration: order.Duration,
                    MealsDaysPerWeek: order.MealsDaysPerWeek,
                    NoOfMealsPerDay: order.NoOfMealsPerDay,
                });

                promise.then(
                    function (payload) {
                        orderId = payload.data;
                        $scope.showMessageSave = true;
                        $timeout(function () {
                            $scope.showMessageSave = false;
                        }, 2000);
                    });
            }
        }

        $scope.Cancel = function () {
            $location.url('/orders');
        };

        $scope.Delete = function (orderId) {
            $scope.showMessageDeleted = false;
            var promise = $http.get('/webapi/OrderApi/DeleteOrder?orderId=' + orderId, {});
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

        $http.get('/webapi/OrderApi/GetAllPackageTypes').success(function (data, status) {
            $scope.packageTypes = data;
        });

        
    }
    ]);


angular
    .module('homer').controller('OrdersController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils',
        function ($scope, ngTableParams, $http, $filter, $location, Utils) {
            $scope.loadingSpinner = true;
            var promise = $http.get('/webapi/OrderApi/GetAllOrders');
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
                { name: 'OrderId', field: 'OrderId', width: '7%', cellTemplate: '<div class="ui-grid-cell-contents"><a href="#/orders/edit/{{row.entity.ClientId}}/{{row.entity.OrderId}}">{{row.entity.OrderId}}</a></div>' },
                { name: 'Start On', field: 'StartOn', width: '13%', cellFilter: 'date:"yyyy-MM-dd HH:mm:ss"' },
                { name: 'Duration', field: 'Duration', width: '8%', },
                { name: 'Client', field: 'ClientName', width: '12%' },
                { name: 'MealsDaysPerWeek', field: 'MealsDaysPerWeek', width: '16%' },
                { name: 'PackageType', field: 'PackageType', width: '15%' },
                { name: 'NoOfMealsPerDay', field: 'NoOfMealsPerDay', width: '15%' },               
                { name: 'Created On', field: 'CreatedOn', width: '14%',cellFilter: 'date:"yyyy-MM-dd HH:mm:ss"' },
            ];


            $scope.gridData.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
            };

        }]);

