'use strict';
app.directive('filterActiveTask', function () {
    return {
        scope: {},
        templateUrl: '/app/views/directives/filterTask.html',
        replace: true,
        controller: 'filterActiveTaskController'
    };
}).controller('filterActiveTaskController', ['$scope', 'filterFactory', 'resourceService', function ($scope, filterFactory, resourceService) {

    $scope.cats = [];
    $scope.filter = {
        text: '',
        task: { cat: {} }
    };

    var loadCats = function () {
        resourceService.get('activetotals', function (data) {
            $scope.cats = data.categories;
        });
    };

    $scope.search = function() {
        filterFactory.setPTaskFilter({
            name: $scope.filter.text,
            task: {
                category: {
                    name: $scope.filter.cat ? $scope.filter.cat.name : ''
                }
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