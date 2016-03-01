'use strict';
app.controller('catsController', ['$scope', 'resourceService', 'filterFactory', '$timeout', function ($scope, resourceService, filterFactory, $timeout) {

    $scope.cats = [];
    $scope.catInEdit = {id: ''};
    $scope.catFilter = filterFactory.getCatFilter();

    filterFactory.subscribe($scope, 'catFilter', function catFilterChanged() {
        $scope.catFilter = filterFactory.getCatFilter();
    });

    $timeout(function() {
        resourceService.get('categories', function(data) {
            $scope.cats = data;
            if (data.length === 0) {
                $scope.addWarning('No categories found');
            }
        });
    }, 100);
}]);