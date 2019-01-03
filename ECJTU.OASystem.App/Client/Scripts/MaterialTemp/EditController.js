(function (app) {
    var EditController = function ($scope, materialTempService) {
        //点击取消
        $scope.cancel = function () {
            $scope.$root.edit.materialTemp = null;
        };
        //点击保存
        $scope.save = function () {

            if ($scope.edit.materialTemp.Id) {
                updateMaterialTemp();
            }
            else {
                createMaterialTemp();
            }
        };
        //更新数据
        var updateMaterialTemp = function () {
            var materialTempupdate = $scope.$root.edit.materialTemp;
            materialTempService.update(materialTempupdate, $scope.$root.edit.materialTemp.BusinessId).then(function (result) {
                console.log(result);
                editMaterial(materialTempupdate);
            });
        };
        //添加数据
        var createMaterialTemp = function () {
            var materialTempadd = $scope.$root.edit.materialTemp;
            materialTempService.create(materialTempadd, $scope.$root.edit.materialTemp.BusinessId).then(function (result) {
                materialTempadd.Id = result;
                addMaterialTemp(materialTempadd);
            });
        };
        //向列表添加数据
        var addMaterialTemp = function (materialTemp) {
            $scope.$root.materialTemps.push(materialTemp);
            $scope.$root.edit.materialTemp = null;
        }
        //更新列表数据
        var editMaterialTemp = function (materialTemp) {
            for (var i = 0; i < $scope.$root.materialTemps.length; i++) {
                if ($scope.$root.materialTemps[i].ClassId == materialTemp.Id) {
                    $scope.$root.materialTemps.splice(i, 1, materialTemp);
                    break;
                }
            }
            $scope.$root.edit.materialTemp = null;
        };
    };
    app.controller("EditController", EditController);
}(angular.module("atTheMaterialTemp")));