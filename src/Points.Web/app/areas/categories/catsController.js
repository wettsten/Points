'use strict';
app.controller('catsController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.cats = [];
    $scope.alerts = [];
    $scope.catInEdit = {id: ''};
    $scope.catFilter = filterFactory.getCatFilter();

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $scope.loadCats = function () {
        resourceService.get('categories');
    };

    resourceService.registerForUpdates('categories', function (data) {
        $scope.cats = data;
        if ($scope.cats.length === 0) {
            $scope.addWarning('No categories found');
        }
    });

    $scope.loadCats();
}]);