angular
    .module('homer')
    .controller('ProductEditController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', '$log', '$interval', '$timeout', 'resizeService', 'FileUploader', 'appSettingsService', 'FirstDateFieldGreaterOrEqualToSecondDateField',
    function ($scope, ngTableParams, $http, $filter, $location, $log, $interval, $timeout, resizeService, FileUploader, appSettingsService, FirstDateFieldGreaterOrEqualToSecondDateField) {
        var productId = $scope.productId;
        var action = $scope.action;
        $scope.currentDateGreaterThanBiddingPeriodEndsOnDate = false;
        var uploader = $scope.uploader = new FileUploader({
            url: '/upload/?parentId=' + $scope.mediaFolderId
        });

        $scope.mediaTypes = 2;

        if (action == 'create') {
            productId = 0;
            $scope.product = {
                BiddingPeriodEndsOn: ''
            };
        }

        if (action == 'edit') {
            var promise = $http.get('/webapi/ProductApi/GetProduct?productId=' + productId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;

                    $scope.product = {
                        ProductId: c.productId,
                        Name: c.Name,
                        Description: c.Description,
                        MinimumPrice: c.MinimumPrice,
                        BiddingPeriod: c.BiddingPeriod,
                        BiddingPeriodEndsOn: c.BiddingPeriodEndsOn != null ? moment(c.BiddingPeriodEndsOn).format('YYYY-MM-DD HH:mm:ss') : null,
                        CategoryId :c.CategoryId,
                        TimeStamp: c.TimeStamp,
                        CreatedOn: c.CreatedOn,
                        CreatedBy: c.CreatedBy,
                        UpdatedBy: c.UpdatedBy,

                    };
                }
              );

        }

        $scope.Save = function (product) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                var promise = $http.post('/webapi/ProductApi/Saveproduct', {                   
                    ProductId: productId,                   
                    Name: product.Name,
                    Description: product.Description,
                    MinimumPrice : product.MinimumPrice,
                    BiddingPeriod : product.BiddingPeriod,
                    CategoryId: product.CategoryId,
                    BiddingPeriodEndsOn: product.BiddingPeriodEndsOn,
                });
                promise.then(
                    function (payload) {
                        productId = payload.data;

                        $scope.mediaFolderId = payload.data.MediaFolderId;

                        uploader.url = '/upload/?parentId=' + $scope.mediaFolderId;
                        $scope.showMessageSave = true;

                        $timeout(function () {
                            $scope.showMessageSave = false;
                        }, 2000);
                    });
            }
        }

        $scope.Delete = function (product) {
            $scope.showMessageDelete = false;
            var promise = $http.get('/webapi/productApi/DeleteProduct?productId=' + productId, {});
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

        $scope.CheckCurrentDateGreaterThanOrEqualToBiddingPeriodEndsOnDate = function () {
            var currentDate = new Date();
            $scope.currentDateFormated = moment(currentDate).format('YYYY-MM-DD HH:mm:ss');            
            if (FirstDateFieldGreaterOrEqualToSecondDateField.isGreaterDate(
                 $scope.currentDateFormated, $scope.product.BiddingPeriodEndsOn) == true) {
                $scope.currentDateGreaterThanBiddingPeriodEndsOnDate = true;
                $scope.form.$valid = false;
            } else {
                $scope.currentDateGreaterThanBiddingPeriodEndsOnDate = false;
            }

        }


        $scope.Cancel = function () {
            $location.url('/products');
        };

        $http.get('/webapi/CategoryApi/GetAllCategories').success(function (data, status) {
            $scope.categories = data;
        });


        //Images section
        $scope.showUploadFiles = true;

        //Load files and display in table
        var promise = $http.get('/webapi/MediaApi/GetFilesInFolder?folderId=' + $scope.mediaFolderId + '&mediaTypes=' + $scope.mediaTypes, {});
        promise.then(
            function (payload) {
                //$scope.folder.Files = payload.data;
                $scope.showFileProperties = false;
                $scope.data = payload.data;

                $scope.tableParams = new ngTableParams({
                    page: 1,            // show first page
                    count: 20,          // count per page
                    sorting: {
                        Name: 'asc'     // initial sorting
                    }
                }, {
                    getData: function ($defer, params) {
                        var filteredData = $filter('filter')($scope.data, $scope.filter);
                        var orderedData = params.sorting() ?
                                            $filter('orderBy')(filteredData, params.orderBy()) :
                                            filteredData;
                        if (orderedData != null) {
                            params.total(orderedData.length);
                            $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        }
                    }
                });

            }
        );


        $scope.Preview = function (media) {
            appSettingsService.getAppSettingsAsync(function (data) {
                $window.open(data.SharedMediaFolder + '\\' + media.MediaId + '\\' + media.Name);
            });
        };

        $scope.basicResize = function (mediaId, src) {
            resizeService.resizeImage(src, { width: 100 }, function (err, image) {
                if (err) {
                    return;
                }
                var basicImgResizedWidth = document.createElement('img');
                basicImgResizedWidth.src = image;
                document.getElementById('img' + mediaId).appendChild(basicImgResizedWidth);
            });
        };

        $scope.CancelFileUpload = function () {
            $scope.showUploadFiles = false;
            uploader.clearQueue();
        }

        $scope.DeleteFile = function (media) {
            var promise = $http.get('/webapi/MediaApi/MarkAsDeleted?mediaId=' + media.MediaId + '&rnd=' + new Date().getTime(), {});
            promise.then(
                function (payload) {

                    var index = $scope.data.indexOf(media)
                    $scope.data.splice(index, 1);
                    $scope.tableParams.reload();

                    $scope.showFileDeleted = true;
                    $scope.showWarningDeleteFile = false;
                    $scope.showFileProperties = false;

                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.showFileDeleted = false;
                        });
                    }, 3000);


                }
            );
        }

        $scope.UploadFiles = function () {
            $scope.showUploadFiles = true;
        }

        var validFileTypes = '|pdf|xml|jpg|jpeg|zip|png|gif|plain|msword|vnd.ms-excel|vnd.openxmlformats-officedocument.wordprocessingml.document|vnd.openxmlformats-officedocument.spreadsheetml.sheet|';
        var validFileExtentions = ["bmp", "gif", "png", "jpg", "jpeg", "doc", "docx", "xls", "xlsx", "pdf", "csv"];
        uploader.filters.push({
            name: 'imageFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                var ext = item.name.substring(item.name.lastIndexOf(".") + 1, item.name.length).toLowerCase();
                if (validFileTypes.indexOf(type) !== -1 && validFileExtentions.indexOf(ext) !== -1)
                    return true;
                else
                    return false
            }
        });
        uploader.filters.push({
            name: 'customFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                return this.queue.length < 10;
            }
        });
        uploader.onCompleteAll = function () {

            //Load files and display in table
            var promise = $http.get('/webapi/MediaApi/GetFilesInFolder?folderId=' + $scope.mediaFolderId + '&mediaTypes=' + $scope.mediaTypes, {});
            promise.then(
                function (payload) {

                    $scope.data = payload.data; 
                    $scope.showFileProperties = false;
                    $scope.tableParams.reload();
                }
            );
        };

    }
    ]);


angular
    .module('homer').controller('ProductController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils', '$interval',
    function ($scope, ngTableParams, $http, $filter, $location, Utils,$interval) {
        var productId = $scope.productId;
        $scope.productId = productId;
        $scope.latestProducts = [];
        $scope.data = [];
        $scope.featuredProducts = [];
        $scope.openProductsForAuctioning = [];
        $scope.closedProductsForAuctioning = [];
        var promise = $http.get('/webapi/productApi/GetAllProducts', {});
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
            }
        );

        $scope.$watch("filter.$", function () {
            if (!Utils.isUndefinedOrNull($scope.tableParams)) {
                $scope.tableParams.reload();
            }
        });

        var latestProductsPromise = $http.get('/webapi/productApi/GetLatestProducts', {});
        latestProductsPromise.then(
            function (payload) {               
                $scope.latestProducts = payload.data;
            }
        );

        var featuredProductsPromise = $http.get('/webapi/productApi/GetFeaturedProducts', {});
        featuredProductsPromise.then(
            function (payload) {               
                $scope.featuredProducts = payload.data;
            }
        );

       
        var showOpenProducts = function (){
        var openProductsForAuctioningPromise = $http.get('/webapi/productApi/GetAllOpenProductsForAuctioning', {});
        openProductsForAuctioningPromise.then(
            function (payload) {
                $scope.openProductsForAuctioning = payload.data;
            }
        );
        }
        showOpenProducts();

        var showClosedProducts = function () {
            var closedProductsForAuctioningPromise = $http.get('/webapi/productApi/GetAllClosedProductsForAuctioning', {});
            closedProductsForAuctioningPromise.then(
                function (payload) {
                    $scope.closedProductsForAuctioning = payload.data;
                }
            );
        }

        showClosedProducts();

        $scope.updateBidStatus = function () {

            var promise = $http.get('/webapi/productApi/GetUpdateBidStatus', {});
            promise.then(
            function (payload) {
                showOpenProducts();
                showClosedProducts();
            });
        }

        $interval(function () {
            $scope.updateBidStatus();
        }, 30000);
    }
    ]);


angular
    .module('homer').controller('MyProductController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils',
    function ($scope, ngTableParams, $http, $filter, $location, Utils) {
        var productId = $scope.productId;
        $scope.productId = productId;

        $scope.data = [];
        var promise = $http.get('/webapi/productApi/GetAllUserProducts', {});
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
            }
        );

        $scope.$watch("filter.$", function () {
            if (!Utils.isUndefinedOrNull($scope.tableParams)) {
                $scope.tableParams.reload();
            }
        });
    }
    ]);

angular
    .module('homer').controller('ProductBidController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils',
    function ($scope, ngTableParams, $http, $filter, $location, Utils) {
        var productId = $scope.productId;
        $scope.productId = productId;

        $scope.data = [];
        var promise = $http.get('/webapi/productApi/GetAllProductBids?productId='+productId, {});
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
            }
        );

        $scope.$watch("filter.$", function () {
            if (!Utils.isUndefinedOrNull($scope.tableParams)) {
                $scope.tableParams.reload();
            }
        });
    }
    ]);

angular
    .module('homer')
    .controller('ProductBidEditController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', '$log', '$interval', '$timeout','Utils',
    function ($scope, ngTableParams, $http, $filter, $location, $log, $interval, $timeout,Utils) {
        var productId = $scope.productId;
        var action = $scope.action;
        var bidId = $scope.bidId;
        var minimumPrice = $scope.minimumPrice;
        

        var promise = $http.get('/webapi/UserApi/GetLoggedInUser', {});
        promise.then(
            function (payload) {
                var c = payload.data;

                $scope.user = {
                    UserName: c.UserName,
                    MediaId: c.MediaId,
                    UserPhoto: c.UserPhoto,
                    FirstName: c.FirstName,
                    LastName: c.LastName,

                };
            }
        );

       

        if (action == 'create') {
            bidId = 0;
        }

        if (action == 'edit') {
            var promise = $http.get('/webapi/ProductApi/GetBid?bidId=' + bidId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;

                    $scope.bid = {
                        BidId: c.BidId,
                        Amount: c.Amount,
                        ProductId: c.ProductId,
                        BidderId: c.BidderId,
                        TimeStamp: c.TimeStamp,
                        CreatedOn: c.CreatedOn,
                        CreatedBy: c.CreatedBy,
                        UpdatedBy: c.UpdatedBy
                    };
                }
              );

        }

        $scope.Save = function (bid) {
            $scope.showMessageSave = false;
            $scope.showMinimumMsg = false;
            if (bid.Amount <= minimumPrice)
            {
                $scope.showMinimumMsg = true;
                $timeout(function () {
                    $scope.showMinimumMsg = false;
                }, 7000);
            } else {
                if ($scope.form.$valid) {
                    var promise = $http.post('/webapi/ProductApi/SaveProductBid', {
                        ProductId: productId,
                        BidId: bidId,
                        Amount: bid.Amount,
                        Status: bid.Status,
                    });
                    promise.then(
                        function (payload) {
                            bidId = payload.data;
                            $scope.showMessageSave = true;

                            $timeout(function () {
                                $scope.showMessageSave = false;
                            }, 2000);
                        });
                }
            }

            
        }

        

        
    }
    ]);


angular
    .module('homer')
    .controller('ProductViewController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', '$log', '$interval', '$timeout', 'resizeService', 'FileUploader', 'appSettingsService',
    function ($scope, ngTableParams, $http, $filter, $location, $log, $interval, $timeout, resizeService, FileUploader, appSettingsService) {
        var productId = $scope.productId;
        var action = $scope.action;

        var uploader = $scope.uploader = new FileUploader({
            url: '/upload/?parentId=' + $scope.mediaFolderId
        });

        $scope.mediaTypes = 2;      

            var promise = $http.get('/webapi/ProductApi/GetProduct?productId=' + productId, {});
            promise.then(
                function (payload) {
                    var c = payload.data;

                    $scope.product = {
                        ProductId: c.productId,
                        Name: c.Name,
                        Description: c.Description,
                        MinimumPrice: c.MinimumPrice,
                        BiddingPeriod: c.BiddingPeriod,
                        BiddingPeriodEndsOn: c.BiddingPeriodEndsOn != null ? moment(c.BiddingPeriodEndsOn).format('YYYY-MM-DD HH:mm:ss') : null,
                        CategoryId: c.CategoryId,
                        TimeStamp: c.TimeStamp,
                        CreatedOn: c.CreatedOn,
                        CreatedBy: c.CreatedBy,
                        UpdatedBy: c.UpdatedBy,
                        SellerPhoneNumber: c.SellerPhoneNumber
                    };
                }
              );


      
        //Images section
            $scope.showUploadFiles = true;

        var promise = $http.get('/webapi/MediaApi/GetFilesInFolder?folderId=' + $scope.mediaFolderId + '&mediaTypes=' + $scope.mediaTypes, {});
        promise.then(
            function (payload) {                
                $scope.showFileProperties = false;
               // $scope.data = payload.data;
                $scope.productImages = payload.data;                
                
            }
        );


        $scope.Preview = function (media) {
            appSettingsService.getAppSettingsAsync(function (data) {
                $window.open(data.SharedMediaFolder + '\\' + media.MediaId + '\\' + media.Name);
            });
        };

        $scope.basicResize = function (mediaId, src) {
            resizeService.resizeImage(src, { width: 100 }, function (err, image) {
                if (err) {
                    return;
                }
                var basicImgResizedWidth = document.createElement('img');
                basicImgResizedWidth.src = image;
                document.getElementById('img' + mediaId).appendChild(basicImgResizedWidth);
            });
        };


      
    }
    ]);



angular
    .module('homer').controller('MyBidsController', ['$scope', 'ngTableParams', '$http', '$filter', '$location', 'Utils',
    function ($scope, ngTableParams, $http, $filter, $location, Utils) {
       
        $scope.data = [];
        var promise = $http.get('/webapi/productApi/GetAllBids', {});
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
            }
        );

        $scope.$watch("filter.$", function () {
            if (!Utils.isUndefinedOrNull($scope.tableParams)) {
                $scope.tableParams.reload();
            }
        });
    }
    ]);

angular
    .module('homer')
    .controller('MyProductSearchController', ['$scope', 'ngTableParams', '$http', '$log', '$filter', 'Utils', 'FirstDateFieldGreaterOrEqualToSecondDateField',
    function ($scope, ngTableParams, $http, $log, $filter, Utils, FirstDateFieldGreaterOrEqualToSecondDateField) {
        $scope.data = [];
        $scope.fromDateGreaterThantoDate = false;
        $scope.totalAmount = 0;
        $scope.messageDisplayed = "";

        $scope.tableParams = new ngTableParams({ page: 1, count: 10, sorting: { ReceivedOn: 'desc' } }, {
            total: $scope.data.length, getData: function ($defer, params) {
                var orderData = params.sorting() ?
                                    $filter('orderBy')($scope.data, params.orderBy()) :
                                    $scope.data;
                $defer.resolve(orderData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        });

        $scope.SearchProducts = function (product) {

            if (Utils.isUndefinedOrNull(product)) {
                product = {};
            }
            $scope.data = [];
            var promise = $http.post('/webapi/productApi/GenerateSoldProductsReport',
                    {
                        FromDate: Utils.isUndefinedOrNull(product.FromDate) ? null : product.FromDate,
                        ToDate: Utils.isUndefinedOrNull(product.ToDate) ? null : product.ToDate,
                        SearchText: product.SearchText
                    });
            promise.then(
             function (payload) {
                 $scope.data = payload.data;
                 $scope.totalAmount = 0;
                 if ($scope.data.length > 0) {
                     $scope.data.forEach(function (product) {
                         $scope.totalAmount += product.AmountSold;
                     })

                     $scope.messageDisplayed = "Total Amount From Products Sold from " + product.FromDate + "  to " + product.ToDate + " is :"
                 }
                 $scope.tableParams.reload();
             });

        }


        $scope.CheckFromDateGreaterThanOrEqualtoToDate = function () {
            if (FirstDateFieldGreaterOrEqualToSecondDateField.isGreaterDate(
                 $scope.product.FromDate, $scope.product.ToDate) == true) {
                $scope.fromDateGreaterThantoDate = true;
                $scope.form.$valid = false;
            } else {
                $scope.fromDateGreaterThantoDate = false;
            }

        }

        $scope.GenerateReportForProductsSoldInPastSevenDays = function () {
            $scope.data = [];
            var promise = $http.get('/webapi/productApi/GenerateReportForProductsSoldInPastSevenDays', {});

            promise.then(
             function (payload) {
                 $scope.totalAmount = 0;
                 $scope.data = payload.data;
                 if ($scope.data.length > 0) {
                     $scope.data.forEach(function (product) {
                         $scope.totalAmount += product.AmountSold;
                     })

                     $scope.messageDisplayed = "Total Amount From Products Sold  In the Past Seven Days is :";
                 }
                 $scope.tableParams.reload();
             });
        }

        $scope.GenerateTodaysProductReport = function () {
            $scope.data = [];
            var promise = $http.get('/webapi/productApi/GenerateTodaysProductReport', {});
            promise.then(
             function (payload) {
                 $scope.data = payload.data;
                 $scope.totalAmount = 0;
                 if ($scope.data.length > 0) {
                     $scope.data.forEach(function (product) {
                         $scope.totalAmount += product.AmountSold;
                     })

                     $scope.messageDisplayed = "Total Amount From Products Sold Today is :";
                 }
                 $scope.tableParams.reload();
             });
        }

        $scope.GenerateYesterdayProductReport = function () {
            $scope.data = [];
            var promise = $http.get('/webapi/productApi/GenerateYesterdayProductReport', {});
            promise.then(
             function (payload) {
                 $scope.data = payload.data;
                 $scope.totalAmount = 0;
                 if ($scope.data.length > 0) {
                     $scope.data.forEach(function (product) {
                         $scope.totalAmount += product.AmountSold;
                     })

                     $scope.messageDisplayed = "Total Amount From Products Sold Yesterday is :";
                 }
                 $scope.tableParams.reload();
             });
        }

        $scope.GenerateCurrentWeekReport = function () {
            $scope.data = [];
            var promise = $http.get('/webapi/productApi/GenerateCurrentWeekReport', {});
            promise.then(
             function (payload) {
                 $scope.data = payload.data;
                 $scope.totalAmount = 0;
                 if ($scope.data.length > 0) {
                     $scope.data.forEach(function (product) {
                         $scope.totalAmount += product.AmountSold;
                     })

                     $scope.messageDisplayed = "Total Amount From Products Sold In This Week is  :";
                 }
                 $scope.tableParams.reload();
             });
        }

        $scope.GenerateCurrentMonthProductReport = function () {
            $scope.data = [];
            var promise = $http.get('/webapi/productApi/GenerateCurrentMonthProductReport', {});
            promise.then(
             function (payload) {
                 $scope.data = payload.data;
                 $scope.totalAmount = 0;
                 if ($scope.data.length > 0) {
                     $scope.data.forEach(function (product) {
                         $scope.totalAmount += product.AmountSold;
                     })

                     $scope.messageDisplayed = "Total Amount From Products Sold In The Current Month is :";
                 }
                 $scope.tableParams.reload();
             });
        }

    }
    ]);