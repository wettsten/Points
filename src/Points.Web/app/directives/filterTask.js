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

    $scope.filter = {
        isOpen: false,
        text: '',
        cat: {}
    };

    $scope.search = function() {
        filterFactory.setTaskFilter({
            name: $scope.filter.text,
            category: {
                name: $scope.filter.cat ? $scope.filter.cat.name : ''
            }
        });
    };

    $scope.clear = function () {
        $scope.filter.text = '';
        $scope.filter.cat = {};
        $scope.search();
    };
}]);