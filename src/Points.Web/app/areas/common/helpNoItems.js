'use strict';
app.directive('helpNoItems', function () {
    return {
        scope: {
            showCategories: '@',
            showTasks: '@',
            showPlanning: '@'
        },
        templateUrl: '/app/views/directives/helpNoItems.html',
        controller: 'helpNoItemsController'
    };
}).controller('helpNoItemsController', ['$scope', function ($scope) {

    $scope.show = {
        categories: (typeof $scope.showCategories === "undefined") ? false : ($scope.showCategories === false) ? false : true,
        tasks: (typeof $scope.showTasks === "undefined") ? false : ($scope.showTasks === false) ? false : true,
        planning: (typeof $scope.showPlanning === "undefined") ? false : ($scope.showPlanning === false) ? false : true
    };
}]);