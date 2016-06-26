
//Applicatoin settings
angular
    .module('homer')
    .factory('appSettingsService', function ($http) {
        return {
            getAppSettingsAsync: function (callback) {
                $http.get('/webapi/AppApi/GetAppSettings').success(callback);
            }
        };
    });

angular
    .module('homer')
    .factory('Utils', function () {
    var service = {
        isUndefinedOrNull: function (obj) {
            return !angular.isDefined(obj) || obj === null;
        }
    }
    return service;
    });

angular
    .module('homer')
    .factory('openDialogService', function ($rootScope) {
        var service = {};
        service.prepForBroadcast = function (selectedText, relatedContentType) {

            this.selectedText = selectedText;
            this.relatedContentType = relatedContentType;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('OpenDialogBroadcastHandler');
        };
        return service;
    });

angular
    .module('homer')
    .factory('EnumLevel', ['$http', function ($http) {
        return {
            Level: {
                Nursery: 2,
                Primary: 5,
                Secondary: 6,
                College: 7,
                University: 8
            }
        }
    }]);


//InsertItem service
angular
    .module('homer')
    .factory('insertItemService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (inPageMediaLinkingType, item) {
            this.Item = item;
            this.inPageMediaLinkingType = inPageMediaLinkingType;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('InsertItemBroadcastHandler');
        };
        return service;
    });


/** 
    Different media types can be assigned to content, 
**/
angular
    .module('homer')
    .factory('EnumInPageMediaLinkingType', function ($http) {
        return {
            Type: {
                Logo: 1,
                Brochure: 2,
                ContentImage: 3,
                Document: 4,
                QuestionImage: 5,
                AnswerImage: 6
            }
        }
    });


angular
    .module('homer')
    .factory('EnumMediaType', function ($http) {
        return {
            Type: {
                Image: 2,
                Document: 3,
            }
        }
    });

//Reload node service
angular
    .module('homer')
    .factory('reloadNodesService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (parentId) {
            this.ParentId = parentId;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('ReloadNodesBroadcastHandler');
        };
        return service;
    });

//Create node service
angular
    .module('homer')
    .factory('createNodeService', function ($rootScope) {
        var service = {};
        service.prepForBroadcast = function (parentId, pageTypeId, suggestedTitle) {
            this.ParentId = parentId;
            this.PageTypeId = pageTypeId;
            this.SuggestedTitle = suggestedTitle;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('CreateNodeBroadcastHandler');
        };
        return service;
    });


//Delete node service
angular
    .module('homer')
    .factory('deleteNodeService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (nodeId) {
            this.NodeId = nodeId;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('DeleteNodeBroadcastHandler');
        };
        return service;
    });

//Sort Nodes service
angular
    .module('homer')
    .factory('sortNodesService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (nodeId) {
            this.NodeId = nodeId;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('SortNodesBroadcastHandler');
        };
        return service;
    });

//Move Node service
angular
    .module('homer')
    .factory('moveNodeService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (nodeId) {
            this.NodeId = nodeId;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('MoveNodeBroadcastHandler');
        };
        return service;
    });


//Copy Node service
angular
    .module('homer')
    .factory('copyNodeService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (nodeId) {
            this.NodeId = nodeId;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('CopyNodeBroadcastHandler');
        };
        return service;
    });

//Publish Node service
angular
    .module('homer')
    .factory('publishNodeService', function ($rootScope) {
        var service = {};

        service.prepForBroadcast = function (nodeId, publishChildren) {
            this.NodeId = nodeId;
            this.PublishChildren = publishChildren;
            this.broadcastItem();
        };

        service.broadcastItem = function () {
            $rootScope.$broadcast('PublishNodeBroadcastHandler');
        };
        return service;
    });

angular
    .module('homer')
    .factory('EnumRelatedContentType', function ($http) {
        return {
            RelatedContentType: {
                RelatedLinks: 1,
                Media: 2
            }
        }
    });

angular
    .module('homer')
    .factory('FileExtentions', function ($http) {
        return {
            GetAllExtentionTypes: function (callback) {
                $http.get('/webapi/MediaApi/GetAllExtentionTypes').success(callback);
            }
        };
    });


angular
    .module('homer')
    .factory('mediaService', function ($http) {
        return {
            getMediaAsync: function (id, callback) {
                $http.get('/webapi/MediaApi/GetMedia?mediaId=' + id + '&rnd=' + new Date().getTime()).success(callback);
            }
        };
    });

angular
    .module('homer')
    .factory('FirstDateFieldGreaterOrEqualToSecondDateField', function () {
        return {
            isGreaterDate: function (firstDate, secondDate) {
                var firstDateGreater = false;

                if ((firstDate !== undefined && firstDate !== null)
                        && (secondDate !== undefined && secondDate !== null)) {
                    if (firstDate.length !== 0 && secondDate.length !== 0) {
                        if (firstDate >= secondDate) {
                            firstDateGreater = true;
                        }
                    }
                } else {
                    firstDateGreater = false;
                }
                return firstDateGreater;
            }
        };


    });