'use strict';
app.directive('planningCat', function () {
    return {
        scope: {
            cat: '=theCat'
        },
        templateUrl: '/app/views/directives/planningCat.html',
        replace: true,
        controller: 'planningCatController'
    };
}).controller('planningCatController', ['$scope', 'catsService', 'authService', '$uibModal', function ($scope, catsService, authService, $uibModal) {

    
}]);