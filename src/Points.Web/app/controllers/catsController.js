'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.cats = [];
    $scope.message = '';
    $scope.editCatId = '';

    $scope.loadCats = function() {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
        }, function (error) {
            $scope.message = 'Error loading data';
        });
    };

    $scope.loadCats();
}]);