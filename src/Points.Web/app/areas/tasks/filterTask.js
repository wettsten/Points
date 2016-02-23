'use strict';
app.directive('filterTask', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/filterTask.html',
        replace: true,
        controller: 'filterTaskController'
    };
}).controller('filterTaskController', ['$scope', 'filterFactory', 'resourceService', function ($scope, filterFactory, resourceService) {

    $scope.cats = [];
    $scope.filter = {
        text: '',
        cat: {}
    };

    var loadCats = function () {
        resourceService.get('categories');
    };

    resourceService.subscribe('categories', function (data) {
        $scope.cats = data;
    });

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

    loadCats();
}]);