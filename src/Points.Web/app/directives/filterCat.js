'use strict';
app.directive('filterCat', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/filterCat.html',
        replace: true,
        controller: 'filterCatController'
    };
}).controller('filterCatController', ['$scope', 'filterFactory', function ($scope, filterFactory) {

    $scope.searchText = '';
    $scope.isExpanded = false;

    $scope.expand = function() {
        $scope.isExpanded = true;
    };

    $scope.collapse = function () {
        $scope.isExpanded = false;
    };

    $scope.search = function() {
        filterFactory.setCatFilter({name:$scope.searchText});
    };
}]);