'use strict';
app.directive('collapsePanel', function() {
    return {
        scope: {
            initOpen: '=',
            headerTitle: '=',
            headerClass: '='
        },
        templateUrl: '/app/views/directives/collapsePanel.html',
        transclude: true,
        controller: "collapsePanelController"
    };
}).controller('collapsePanelController', ['$scope', function ($scope) {

    $scope.panelClass = $scope.headerClass;
    $scope.isOpen = $scope.initOpen;
}]);