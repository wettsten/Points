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

    resourceService.subscribe('categories', function (data) {
        $scope.cats = data;
        if (data.length === 0) {
            if ($scope.addWarning) {
                $scope.addWarning('No categories found');
            }
        }
    });

    $scope.loadCats();
}]);