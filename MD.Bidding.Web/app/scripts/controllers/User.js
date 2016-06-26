

angular
    .module('homer')
    .controller('UserEditController', ['$scope', '$http', '$filter', '$location', '$log', '$timeout', '$modal', 'insertItemService', 'EnumInPageMediaLinkingType', 'EnumMediaType',
    function ($scope, $http, $filter, $location, $log, $timeout, $modal, insertItemService, EnumInPageMediaLinkingType, EnumMediaType) {
        var Id = $scope.Id;
        $scope.roles = [];
        $scope.checkedRoles = [];
        console.log($scope.checkedRoles);
        var action = $scope.action;
            
        var promise = $http.get('/webapi/UserApi/GetUser', {});
        promise.then(
            function (payload) {
                var c = payload.data;

                $scope.user = {
                    Id: c.Id,
                    FirstName:c.FirstName,
                    LastName :c.LastName,
                    Email :c.Email,
                    UserName :c.UserName ,
                    MiddleName :c.MiddleName,
                    Address :c.Address,
                    PhoneNumber :c.PhoneNumber,
                    Mobile :c.Mobile,
                    Town :c.Town,
                    CityId :c.CityId,
                    CountryId :c.CountryId,
                    GenderId :c.GenderId,
                    CreatedBy:c.CreatedBy,
                    UpdatedBy :c.UpdatedBy,
                    TimeStamp :c.TimeStamp,
                    DateOfBirth :c.DateOfBirth,
                    CreatedOn  :c.CreatedOn,
                    MediaId: c.MediaId,
                    UserRoles: c.UserRoles,
                    UserPhoto: c.UserPhoto,
                };
            }
        );

        if (action == 'create') {
            Id = 0;
            $scope.user = {
                UserRoles: []
            };
        }

       
        $scope.Save = function (user) {
            $scope.showMessageSave = false;
            if ($scope.form.$valid) {
                console.log("selected roles"+ user.UserRoles);
                var promise = $http.post('/webapi/SchoolUserApi/SaveSchoolUser', {
                    Id: user.Id,
                    FirstName: user.FirstName,
                    LastName: user.LastName,
                    Email: user.Email,
                    UserName: user.UserName,
                    MiddleName: user.MiddleName,
                    Address: user.Address,
                    PhoneNumber: user.PhoneNumber,
                    UserRoles: user.UserRoles,
                    MediaId: user.UserPhotoId,
                    PasswordHash: user.Password
                });

                promise.then(
                    function (payload) {
                        user = payload.data;
                      
                        if (user.EmailExists == true) {
                            $scope.showMessageEmailExists = true;

                            $timeout(function () {
                                $scope.showMessageEmailExists = false;
                            }, 4000);
                        }

                        else {
                            $scope.showMessageSave = true;

                            $timeout(function () {
                                $scope.showMessageSave = false;
                            }, 2000);
                        }
                        

                    });
            }
        }

        $scope.DeletePhoto = function () {
            $scope.user.UserPhoto = {};
            $scope.user.UserPhotoId = null
        };

        $scope.ManagePhoto = function (user) {
            $scope.modalInstance = $modal.open({
                templateUrl: '/app/views/modal/modalContent.html?rnd=' + new Date().getTime(),
                size: 'large',
                controller: 'ModalInstanceCtrl',
                resolve: {
                    items: function () {

                        $scope.dataItems = {
                            showLinkPickerBtn2: true,
                            showLinkPicker2: true,
                            inPageMediaLinkingType: EnumInPageMediaLinkingType.Type.UserPhoto,
                            mediaTypes: EnumMediaType.Type.Image
                        }
                        return $scope.dataItems;
                    },
                },
                windowClass: 'contentModalBox'
            });
        };

        $scope.$on('InsertItemBroadcastHandler', function () {
            if (insertItemService.inPageMediaLinkingType == EnumInPageMediaLinkingType.Type.UserPhoto) {
                $scope.user.UserPhotoId = insertItemService.Item.MediaId
                $scope.user.UserPhoto = insertItemService.Item;
            }
            $scope.modalInstance.close();
        });

        $scope.checkPass = function () {
            var pass1 = document.getElementById('pass1');
            var pass2 = document.getElementById('pass2');
    
            var message = document.getElementById('confirmMessage');

            var goodColor = "#66cc66";
            var badColor = "#ff6666";

            if (pass1.value == pass2.value) {           
                message.style.color = goodColor;
                $scope.showMessagePasswordMatch = true;

                $scope.showMessagePasswordDoNotMatchMatch = false;
                $timeout(function () {
                    $scope.showMessagePasswordMatch = false;
                }, 2000);
            } else {
                message.style.color = badColor;
                $scope.showMessagePasswordDoNotMatchMatch = true;
            }
        }
  
        var promise = $http.get('/webapi/UserApi/GetAllRoles', {});
        promise.then(
            function (payload) {
                $scope.roles = payload.data;
            });
    }
    ]);