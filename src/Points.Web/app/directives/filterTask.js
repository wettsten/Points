'use strict';
app.directive('filterTask', function () {
    return {
        scope: {
            cats: '='
        },
        templateUrl: '/app/views/directives/filterTask.html',
        replace: true,
        controller: 'filterTaskController'
    };
}).controller('filterTaskController', ['$scope', 'filterFactory', function ($scope, filterFactory) {

    $scope.searchText = '';
    $scope.isExpanded = false;

    $scope.expand = function() {
        $scope.isExpanded = true;
    };

    $scope.collapse = function () {
        $scope.isExpanded = false;
    };

    $scope.search = function() {
        filterFactory.setTaskFilter({
            name: $scope.searchText,
            category: {
                name: $scope.searchCat ? $scope.searchCat.name : ''
            }
        });
    };

    $scope.clear = function () {
        $scope.searchText = '';
        $scope.searchCat = null;
        $scope.search();
    };
}]);