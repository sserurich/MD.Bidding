var app = angular.module('myApp', []);

app.controller('MessageCountController', function ($scope) {
 
    $scope.inputMessage = "";
    $scope.inputMessage.length = 0;
})