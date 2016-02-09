﻿'use strict';
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
        isOpen: false,
        text: '',
        cat: {}
    };

    var loadCats = function () {
        $scope.cats = resourceService.get('categories');
    };

    resourceService.registerForUpdates('categories', function (data) {
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