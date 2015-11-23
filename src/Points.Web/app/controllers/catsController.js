'use strict';
app.controller('catsController', ['$scope', 'catsService', function ($scope, catsService) {

    $scope.cats = [];
    $scope.message = '';

    $scope.loadCats = function() {
        catsService.getCats().then(function(results) {
            $scope.cats = results.data;
            //$scope.$apply();
        }, function(error) {
            //alert(error.data.message);
        });
    };

    $scope.loadCats();
}]);