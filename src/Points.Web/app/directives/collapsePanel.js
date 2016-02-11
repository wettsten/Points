'use strict';
app.directive('collapsePanel', function() {
    return {
        scope: {
            initOpen: '=',
            headerTitle: '='
        },
        templateUrl: '/app/views/directives/collapsePanel.html',
        transclude: true,
        controller: "collapsePanelController"
    };
}).controller('collapsePanelController', ['$scope', function ($scope) {

    $scope.isOpen = $scope.initOpen;
}]);