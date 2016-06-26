 function configState($stateProvider, $urlRouterProvider, $compileProvider) {

    // Optimize load start with remove binding information inside the DOM element
    $compileProvider.debugInfoEnabled(false);
    
    $urlRouterProvider
        .otherwise("/dashboard");

    $stateProvider
        .state('dashboard', {
            url: "/dashboard",
            templateUrl: "/app/views/dashboard.html",
            data: {
                pageTitle: 'Dashboard',
            }
        })
   
    .state('profile', {
        url: "/profile",
        templateUrl: "/app/views/_common/profile.html",
        data: {
            pageTitle: 'Profile'
        }
    })

     .state('categories', {
         url: "/categories",
         templateUrl: "/app/views/categories/list.html",
         data: {
             pageTitle: 'Categories'
         }
     })

    .state('category-edit', {
        url: "/categories/:action/:categoryId",
        templateUrl: "/app/views/categories/edit.html",
        data: {
            pageTitle: 'Category',
            pageDesc: ''
        },
        controller: function ($scope, $stateParams) {
            $scope.action = $stateParams.action;
            $scope.categoryId = $stateParams.categoryId;           
        }
    })

         .state('products', {
             url: "/products",
             templateUrl: "/app/views/product/list.html",
             data: {
                 pageTitle: 'Products'
             }
         })

    .state('product-edit', {
        url: "/products/:action/:productId/:mediaFolderId",
        templateUrl: "/app/views/product/edit.html",
        data: {
            pageTitle: 'Product',
            pageDesc: ''
        },
        controller: function ($scope, $stateParams) {
            $scope.action = $stateParams.action;
            $scope.productId = $stateParams.productId;
            $scope.mediaFolderId = $stateParams.mediaFolderId;
        }
    })

    .state('product-bids', {
        url: "/product/bids/:productId",
        templateUrl: "/app/views/product/productbids.html",
        data: {
            pageTitle: 'Product',
            pageDesc: ''
        },
        controller: function ($scope, $stateParams) {
            $scope.action = $stateParams.action;
            $scope.productId = $stateParams.productId;           
        }
    })

         .state('product-bid-edit', {
             url: "/product/bids/:productId/:action/:bidId",
             templateUrl: "/app/views/product/product-bid-edit.html",
             data: {
                 pageTitle: 'Product Bid Edit',
                 pageDesc: ''
             },
             controller: function ($scope, $stateParams) {
                 $scope.action = $stateParams.action;
                 $scope.productId = $stateParams.productId;
                 $scope.bidId = $stateParams.bidId
             }
         })
    

     .state('reports', {
         url: "/reports",
         templateUrl: "/app/views/reports/product-report.html",
         data: {
             pageTitle: 'Product Reports'
         }
     })
     //.state('bidsreport', {
     //    url: "/reports/bids",
     //    templateUrl: "/app/views/reports/bid-report.html",
     //    data: {
     //        pageTitle: 'Bid Reports'
     //    }
     //})
    
    

}

angular
    .module('homer')
    .config(configState)
    .run(function($rootScope, $state, editableOptions,$document) {
        $rootScope.$state = $state;
        editableOptions.theme = 'bs3';

        //Idle checker
        var d = new Date();
        var n = d.getTime();  //n in ms

        $rootScope.idleEndTime = n + (20 * 60 * 1000); //set end time to 20 min from now
        $document.find('body').on('mousemove keydown DOMMouseScroll mousewheel mousedown touchstart', checkAndResetIdle); //monitor events

        function checkAndResetIdle() //user did something
        {
            var d = new Date();
            var n = d.getTime();  //n in ms

            if (n > $rootScope.idleEndTime) {
                $document.find('body').off('mousemove keydown DOMMouseScroll mousewheel mousedown touchstart'); //un-monitor events
                document.location.href = '/Account/LogOff';
            }
            else {
                $rootScope.idleEndTime = n + (20 * 60 * 1000); //reset end time
            }
        }
    });


