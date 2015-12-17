'use strict';
app.directive('newCatRow', function() {
    return {
        scope: {
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/newCatRow.html',
        replace: true,
        controller: 'newCatRowController'
    };
}).controller('newCatRowController', ['$scope', 'catsService', 'authService', '$timeout', function ($scope, catsService, authService, $timeout) {

    $scope.addCatData = {};

    $scope.clearAddData = function () {
        $scope.addCatData = {};
    };

    $scope.addCat = function () {
        $scope.addCatData.userId = authService.authentication.userId;
        catsService.addCat($scope.addCatData).then(function (response) {
            $scope.clearAddData();
            $timeout(function () {
                $scope.$emit('refreshCats');
                $scope.addAlert({ type: 'success', msg: 'Category successfully added' });
            }, 100);
            },
         function (err) {
             $scope.addAlert({ type: 'danger', msg: err.statusText });
         });
    };
}]);