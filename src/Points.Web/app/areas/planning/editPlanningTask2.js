'use strict';
app.directive('editPlanningTaskTwo', function () {
    return {
        scope: {
            task: '=theTask',
            addAlert: '&'
        },
        templateUrl: '/app/views/directives/editPlanningTask2.html',
        replace: true,
        controller: 'editPlanningTask2Controller'
    };
}).controller('editPlanningTask2Controller', ['$scope', 'resourceService', '$uibModal', function ($scope, resourceService, $uibModal) {

    $scope.enums = {};

    var loadEnums = function () {
        resourceService.get('enums');
    };

    resourceService.registerForUpdates('enums', function (data) {
        $scope.enums = data;
    });

    $scope.startEdit = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/app/views/partials/editPlanningTask.html',
            controller: 'editPlanningTaskModal',
            size: 'lg',
            resolve: {
                task: angular.copy($scope.task)
            }
        });
        modalInstance.result.then(
            function (result) {
                if (result) {
                    resourceService.edit('planningtasks', result).then(
                        function (response) {
                            $scope.addAlert({ type: 'success', msg: 'Task successfully updated' });
                        },
                        function (err) {
                            $scope.addAlert({ type: 'danger', msg: err.data.message });
                        });
                }
            });
    };

    $scope.delete = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/app/views/partials/confirmDelete.html',
            controller: 'confirmDeleteController',
            size: 'sm',
            resolve: {
                item: function () {
                    return {
                        name: $scope.task.task.name,
                        id: $scope.task.id
                    };
                }
            }
        });

        modalInstance.result.then(
            function (result) {
                if (result !== 'cancel') {
                    resourceService.delete('planningtasks',$scope.task.id).then(
                        function (response) {
                            $scope.$emit('refreshTasks');
                            $scope.addAlert({ type: 'success', msg: 'Task successfully deleted' });
                        },
                        function (err) {
                            $scope.addAlert({ type: 'danger', msg: err.data.message });
                        });
                }
        });
    };

    loadEnums();
}]);