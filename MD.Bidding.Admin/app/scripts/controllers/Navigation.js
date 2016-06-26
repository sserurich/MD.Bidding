angular
    .module('homer').controller('NavController', ['$scope', '$http',
    function ($scope, $http) {

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
    }
    ]);

