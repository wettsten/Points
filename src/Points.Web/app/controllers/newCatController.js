'use strict';
app.controller('newCatController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.addCatData = {};

    $scope.clearAddData = function () {
        $scope.addCatData = {};
    };

    $scope.addCat = function () {
        $scope.addCatData.userId = authService.authentication.userId;
        catsService.addCat($scope.addCatData).then(function (response) {
            $scope.clearAddData();
            $scope.loadCats();
            },
         function (err) {
             $scope.$parent.message = err.status + ' ' + err.data.message;
         });
    };
}]);