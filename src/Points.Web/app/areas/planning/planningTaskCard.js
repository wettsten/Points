'use strict';
app.directive('planningTaskCard', function () {
    return {
        scope: {
            task: '=theTask',
            addSuccess: '&',
            addError: '&'
        },
        templateUrl: '/app/views/directives/planningTaskCard.html',
        replace: true,
        controller: 'planningTaskCardController'
    };
}).controller('planningTaskCardController', ['$scope', 'resourceService', 'modalService', function ($scope, resourceService, modalService) {

    $scope.enums = {};

    var loadEnums = function () {
        resourceService.get('enums', function (data) {
            $scope.enums = data;
        });
    };

    $scope.startEdit = function () {
        modalService.newModal('editPlanningTask', angular.copy($scope.task), 'lg', 
            function (result) {
                $scope.addSuccess({ msg: "Task '{0}' successfully updated".format(result.name) });
            }
        );
    };

    $scope.delete = function () {
        var name = $scope.task.name;
        modalService.newModal('confirmDelete', { name: $scope.task.name, id: $scope.task.id }, 'sm', 
            function (result) {
                resourceService.delete('planningtasks',$scope.task.id).then(
                    function (response) {
                        $scope.addSuccess({ msg: "Task '{0}' successfully deleted".format(name) });
                    },
                    function (err) {
                        $scope.addError({ msg: err.data.message });
                    }
                );
            }
        );
    };

    loadEnums();
}]);