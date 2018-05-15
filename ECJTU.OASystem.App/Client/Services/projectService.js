(function (app) {
    var projectService = function ($http, projectApiUrl) {
        var getAll = function () {
            return $http.get(projectApiUrl + "GetProjectList");
        };

        var getById = function (id) {
            return $http.get(projectApiUrl + "GetProjectById/" + id);
        };

        var update = function (Project) {
            return $http.put(projectApiUrl + "UpdateProject/" + Project.ProjectId, Project);
        };

        var create = function (Project,userName) {
            console.log(Project);
            return $http.post(projectApiUrl + "CreateProject", { Project: Project, userName: userName});
        };

        var destroy = function (Project) {
            return $http.delete(projectApiUrl + "DeleteProjectById/" + Project.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("projectService", projectService);
}(angular.module("atTheProject")))