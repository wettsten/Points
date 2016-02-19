'use strict';
app.directive('editPlanningTask', function () {
    return {
        scope: {
            task: '=theTask',
            addAlert: '&'
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

    resourceService.registerForUpdates('enums', function (data) {
        $scope.enums = data;
    });

    $scope.startEdit = function () {
        modalService.newModal('editPlanningTask', angular.copy($scope.task), 'lg', 
            function (result) {
                if (result) {
                    resourceService.edit('planningtasks', result).then(
                        function (response) {
                            $scope.addAlert({ type: 'success', msg: 'Task successfully updated' });
                        },
                        function (err) {
                            $scope.addAlert({ type: 'danger', msg: err.data.message });
                        }
                    );
                }
            }
        );
    };

    $scope.delete = function () {
        modalService.newModal('confirmDelete', { name: $scope.task.name, id: $scope.task.id }, 'sm', 
            function (result) {
                resourceService.delete('planningtasks',$scope.task.id).then(
                    function (response) {
                        $scope.$emit('refreshTasks');
                        $scope.addAlert({ type: 'success', msg: 'Task successfully deleted' });
                    },
                    function (err) {
                        $scope.addAlert({ type: 'danger', msg: err.data.message });
                    }
                );
            }
        );
    };

    loadEnums();
}]);