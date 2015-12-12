'use strict';
app.controller('catsController', ['$scope', 'catsService', 'authService', 'filterFactory', function ($scope, catsService, authService, filterFactory) {

    $scope.cats = [];
    $scope.message = '';
    $scope.editCatId = '';
    $scope.catFilter = filterFactory.getCatFilter();

    $scope.loadCats = function() {
        catsService.getCatsByUser(authService.authentication.userId).then(function (results) {
            $scope.cats = results.data;
        }, function (error) {
            $scope.message = 'Error loading data';
        });
    };

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $scope.loadCats();
}]);