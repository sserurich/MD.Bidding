angular
    .module('homer')
    .controller('CategoryEditController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', '$log', '$interval', '$timeout',
    function ($scope, ngTableParams, $http, $filter, $location, $log, $interval, $timeout) {
        var categoryId = $scope.categoryId;
        var action = $scope.action;

        if (action == 'create') {
            categoryId = 0;
        }

        if (action == 'edit') {
            var promise = $http.get('/webapi/CategoryApi/GetCategory?categoryId=' + categoryId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;

                    $scope.category = {
                        CategoryId: c.CategoryId,
                        Name: c.Name,
                        Description: c.Description,                        
                        TimeStamp: c.TimeStamp,
                        CreatedOn: c.CreatedOn,
                        CreatedBy: c.CreatedBy,
                        UpdatedBy: c.UpdatedBy
                    };
                }
              );

        }

        $scope.Save = function (category) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/CategoryApi/SaveCategory', {
                    CategoryId: categoryId,
                    Name: category.Name,
                    Description: category.Description
                });
                promise.then(
                    function (payload) {
                        categoryId = payload.data;
                        $scope.showMessageSave = true;

                        $timeout(function () {
                            $scope.showMessageSave = false;
                        }, 2000);
                    });
            }
        }

        $scope.Delete = function (category) {
            $scope.showMessageDelete = false;
            var promise = $http.get('/webapi/CategoryApi/DeleteCategory?categoryId=' + categoryId, {});
            promise.then(
                function (payload) {
                    $scope.showMessageDeleted = true;
                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.showMessageDeleted = false;
                        });
                    }, 2000);
                    $scope.showMessageDeleteFailed = false;
                },

                function (errorPayload) {
                    $scope.showMessageDeleteFailed = true;
                }
            );

        }



        $scope.Cancel = function () {
            $location.url('/categories');
        };

    }
    ]);


angular
    .module('homer').controller('CategoryController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils',
    function ($scope, ngTableParams, $http, $filter, $location, Utils) {
        var categoryId = $scope.categoryId;
        $scope.categoryId = categoryId;

        $scope.mainCategories = [];
        $scope.data = [];
        var promise = $http.get('/webapi/CategoryApi/GetAllCategories', {});
        promise.then(
            function (payload) {
                $scope.data = payload.data;
                $scope.tableParams = new ngTableParams({
                    page: 1,
                    count: 20,
                    sorting: { CreatedOn: 'desc' }
                }, {
                    getData: function ($defer, params) {
                        var filteredData = $filter('filter')($scope.data, $scope.filter);
                        var orderedData = params.sorting() ?
                                            $filter('orderBy')(filteredData, params.orderBy()) :
                                            filteredData;

                        params.total(orderedData.length);
                        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    },
                    $scope: $scope
                });

                $scope.parentCategories = $filter('filter')($scope.data, {ParentId :!null});
            }
        );

        $scope.$watch("filter.$", function () {
            if (!Utils.isUndefinedOrNull($scope.tableParams)) {
                $scope.tableParams.reload();
            }
        });

        var mainCategoriesPromise = $http.get('/webapi/CategoryApi/GetAllMainCategories', {});
        mainCategoriesPromise.then(
            function (payload) {
                $scope.mainCategories = payload.data;
            }
        );
       
    }
    ]);

