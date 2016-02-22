'use strict';
app.directive('editPlanningTask', function () {
    return {
        scope: {
            task: '=theTask',
            addSuccess: '&',
            addError: '&'
        },
        templateUrl: '/app/views/directives/editPlanningTask.html',
        replace: true,
        controller: 'editPlanningTaskController'
    };
}).controller('editPlanningTaskController', ['$scope', 'resourceService', 'modalService', function ($scope, resourceService, modalService) {

    $scope.enums = {};

    var loadEnums = function () {
        resourceService.get('enums');
    };

    resourceService.subscribe('enums', function (data) {
        $scope.enums = data;
    });

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