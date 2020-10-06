"use strict";

var _phaser = _interopRequireDefault(require("phaser"));

var _game = _interopRequireDefault(require("./scenes/game"));

var _boot = _interopRequireDefault(require("./scenes/boot"));

var _menu = _interopRequireDefault(require("./scenes/menu"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

var config = {
  type: _phaser["default"].AUTO,
  width: 480,
  height: 380,
  pixelArt: true,
  zoom: 2,
  scene: [_menu["default"], _boot["default"], _game["default"]]
};
window.addEventListener('load', function () {
  new _phaser["default"].Game(config);
});