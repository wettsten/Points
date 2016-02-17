'use strict';
app.directive('planningCat', function () {
    return {
        scope: {
            cat: '=theCat',
            taskInEdit: '=',
            addAlertInt: '&addAlert'
        },
        templateUrl: '/app/views/directives/planningCat.html',
        replace: true,
        controller: 'planningCatController'
    };
}).controller('planningCatController', ['$scope', function ($scope) {

    $scope.addAlert = function (type, msg) {
        $scope.addAlertInt({ type: type, msg: msg });
    };
}]);