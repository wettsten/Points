'use strict';
app.controller('newCatController', ['$scope', 'catsService', function ($scope, catsService) {

    $scope.addCatData = {};

    $scope.clearAddData = function () {
        $scope.addCatData = {};
    };

    $scope.addCat = function () {
        catsService.addCat($scope.addCatData).then(function (response) {
            $scope.clearAddData();
            $scope.loadCats();
            },
         function (err) {
             $scope.$parent.message = err.status + ' ' + err.data.message;
         });
    };
}]);