 function configState($stateProvider, $urlRouterProvider, $compileProvider) {

    // Optimize load start with remove binding information inside the DOM element
    $compileProvider.debugInfoEnabled(false);
    
    // Set default state
    $urlRouterProvider
        .otherwise("/dashboard");

    $stateProvider
          // Dashboard - Main page
        .state('dashboard', {
            url: "/dashboard",
            templateUrl: "/app/views/dashboard.html",
            data: {
                pageTitle: 'Dashboard',
            }
        })
   

              // User Profile page
    .state('profile', {
        url: "/profile",
        templateUrl: "/app/views/_common/profile.html",
        data: {
            pageTitle: 'Profile'
        }
    })


    .state('orders', {
        abstract: true,
        url: "/orders",
        templateUrl: "/app/views/_common/content_empty.html",
        data: {
            pageTitle: 'Orders'
        }
    })
    .state('orders.list', {
        url: "/list",
        templateUrl: "/app/views/orders/index.html",
        data: {
            pageTitle: 'Orders',
        },
        controller: function ($scope, $stateParams) {

        }
    })

   .state('order-edit', {
         url: "/orders/:action/:clientId/:orderId",
            templateUrl: "/app/views/orders/edit.html",
            data: {
                pageTitle: 'Order Edit',
                pageDesc: ''
            },
            controller: function ($scope, $stateParams) {
                $scope.action = $stateParams.action;
                $scope.orderId = $stateParams.orderId;
                $scope.clientId = $stateParams.clientId;
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

        .state('products-open-for-biddiing', {
            url: "/open/products/bidding",
            templateUrl: "/app/views/product/products-open-for-bidding.html",
            data: {
                pageTitle: 'Products Open For Bidding'
            }
        })

        .state('products-closed-for-biddiing', {
            url: "/closed/products/bidding",
            templateUrl: "/app/views/product/products-closed-for-bidding.html",
            data: {
                pageTitle: 'Products Open For Bidding'
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

    .state('product-view', {
        url: "/products/:action/:productId/:mediaFolderId",
        templateUrl: "/app/views/product/product-detail.html",
        data: {
            pageTitle: 'Product Details',
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
              url: "/product/bids/:productId/:action/:bidId/:minimumPrice/:biddingDateEndsOn",
              templateUrl: "/app/views/product/product-bid-edit.html",
              data: {
                  pageTitle: 'Product Bid Edit',
                  pageDesc: ''
              },
              controller: function ($scope, $stateParams) {
                  $scope.action = $stateParams.action;
                  $scope.productId = $stateParams.productId;
                  $scope.bidId = $stateParams.bidId;
                  $scope.minimumPrice = $stateParams.minimumPrice;
                  $scope.biddingDateEndsOn = $stateParams.biddingDateEndsOn;
              }
          })


       .state('bids', {
           url: "/myproduct/bids",
           templateUrl: "/app/views/bid/list.html",
           data: {
               pageTitle: 'My Bids'
           }
       })

     .state('reports', {
         url: "/reports",
         templateUrl: "/app/views/reports/product-report.html",
         data: {
             pageTitle: 'Product Reports'
         }
     })
     .state('bidsreport', {
         url: "/reports/bids",
         templateUrl: "/app/views/reports/bid-report.html",
         data: {
             pageTitle: 'Bid Reports'
         }
     })
    

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


