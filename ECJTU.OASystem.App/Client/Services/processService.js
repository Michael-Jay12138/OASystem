(function (app) {
    var processService = function ($http, processApiUrl) {
        var getAll = function () {
            return $http.get(processApiUrl + "GetProcessList");
        };

        var getById = function (id) {
            return $http.get(processApiUrl + "GetProcessById/" + id);
        };

        var update = function (Process) {
            return $http.put(processApiUrl + "UpdateProcess/" + Process.ProcessId, Process);
        };

        var create = function (Process) {
            console.log(Process);
            return $http.post(processApiUrl + "CreateProcess/", Process);
        };

        var destroy = function (Process) {
            return $http.delete(processApiUrl + "DeleteProcessById/" + Process.Id);
        };
        return {
            getAll: getAll,
            getById: getById,
            update: update,
            create: create,
            destroy: destroy
        };
    };
    app.factory("processService", processService);
}(angular.module("atTheProcess")))