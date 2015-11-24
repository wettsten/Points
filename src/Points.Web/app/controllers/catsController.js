'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.cats = [];
    $scope.message = '';

    $scope.loadCats = function() {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
            //$scope.$apply();
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.loadCats();
}]);