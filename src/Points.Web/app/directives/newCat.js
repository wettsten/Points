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
}).controller('newCatController', ['$scope', 'catsService', '$timeout', 'authService', function ($scope, catsService, $timeout, authService) {

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