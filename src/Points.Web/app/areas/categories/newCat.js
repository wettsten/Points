'use strict';
app.directive('newCat', function() {
    return {
        scope: {
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/newCat.html',
        replace: true,
        controller: 'newCatController'
    };
}).controller('newCatController', ['$scope', 'resourceService', '$timeout', function ($scope, resourceService, $timeout) {

    $scope.addCatData = {};

    $scope.clearAddData = function () {
        $scope.addCatData = {};
    };

    $scope.addCat = function () {
        resourceService.add('categories', $scope.addCatData).then(
            function (response) {
                $scope.clearAddData();
                $timeout(
                    function () {
                        $scope.addAlert({ type: 'success', msg: 'Category successfully added' });
                    }, 100
                );
            },
            function (err) {
                $scope.addAlert({ type: 'danger', msg: err.statusText });
            }
        );
    };
}]);