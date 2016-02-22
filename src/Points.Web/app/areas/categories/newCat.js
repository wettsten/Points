'use strict';
app.directive('newCat', function() {
    return {
        scope: {
            addSuccess: '&',
            addError: '&'
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
                var name = $scope.addCatData.name;
                $scope.clearAddData();
                $timeout(
                    function () {
                        $scope.addSuccess({ msg: "Category '" + name + "' successfully added" });
                    }, 100
                );
            },
            function (err) {
                $scope.addError({ msg: err.data.message });
            }
        );
    };
}]);