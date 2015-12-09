'use strict';
app.directive('newCatRow', function() {
    return {
        scope: {},
        templateUrl: '/app/views/newCatRow.html',
        replace: true,
        controller: 'newCatRowController'
    };
}).controller('newCatRowController', ['$scope', 'catsService', 'authService', function ($scope, catsService, authService) {

    $scope.addCatData = {};

    $scope.clearAddData = function () {
        $scope.addCatData = {};
    };

    $scope.addCat = function () {
        //if ($scope.addHasError('addName')) {
        //    $scope.$parent.message = 'Name is required!';
        //    return;
        //}
        $scope.addCatData.userId = authService.authentication.userId;
        catsService.addCat($scope.addCatData).then(function (response) {
            $scope.clearAddData();
            $scope.$parent.loadCats();
            },
         function (err) {
             $scope.$parent.message = err.data.message;
         });
    };

    //$scope.addHasError = function (field, validation) {
    //    if (validation) {
    //        return $scope.addForm[field].$error[validation];
    //    }
    //    return $scope.addForm[field].$invalid;
    //};
}]);