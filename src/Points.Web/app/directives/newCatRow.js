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
}).controller('newCatRowController', ['$scope', 'catsService', '$timeout', 'authService', function ($scope, catsService, $timeout, authService) {

    $scope.allowEditPublic = authService.authentication.allowEditPublic;
    $scope.addCatData = {
        isPrivate: !$scope.allowEditPublic
    };

    $scope.clearAddData = function () {
        $scope.addCatData = {
            isPrivate: !$scope.allowEditPublic
    };
    };

    $scope.addCat = function () {
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