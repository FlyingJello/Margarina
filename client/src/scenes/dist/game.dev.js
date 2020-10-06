"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;

var _phaser = require("phaser");

var _tiles_sewers = _interopRequireDefault(require("../assets/sprites/terrain/tiles_sewers.png"));

var _test_level = _interopRequireDefault(require("../assets/levels/test_level.json"));

var _ranger = _interopRequireDefault(require("../assets/sprites/ranger.png"));

var _actor = _interopRequireDefault(require("../actor"));

var _player = _interopRequireDefault(require("../player"));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { "default": obj }; }

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }

var GameScene =
/*#__PURE__*/
function (_Scene) {
  _inherits(GameScene, _Scene);

  function GameScene() {
    var _this;

    _classCallCheck(this, GameScene);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(GameScene).call(this, {
      type: Phaser.AUTO,
      key: 'GameScene',
      physics: {
        "default": 'arcade',
        arcade: {
          gravity: {
            y: 0
          }
        }
      }
    }));
    _this.actors = [];
    _this.playerId = "69";
    _this.mapId = "testmap";
    return _this;
  }

  _createClass(GameScene, [{
    key: "init",
    value: function init(_ref) {
      var connection = _ref.connection;
      this.connection = connection;
    }
  }, {
    key: "preload",
    value: function preload() {
      this.load.image('tiles', _tiles_sewers["default"]);
      this.load.tilemapTiledJSON('map', _test_level["default"]);
      this.load.spritesheet('player', _ranger["default"], {
        frameWidth: 16,
        frameHeight: 16
      });
    }
  }, {
    key: "create",
    value: function create() {
      var _this2 = this;

      this.map = this.make.tilemap({
        key: 'map'
      });
      var tileset = this.map.addTilesetImage('sewers', 'tiles', 16, 16);
      var layers = this.map.layers.filter(function (layer) {
        return layer.visible;
      }).map(function (layer) {
        return _this2.map.createStaticLayer(layer.name, tileset, 0, 0);
      });
      layers.find(function (tileLayer) {
        return tileLayer.layer.name === "top";
      }).setDepth(69);
      var player = new _player["default"](this, this.playerId, "Doug", {
        x: 0,
        y: 0
      });
      this.actors.push(player);
      this.cameras.main.setBounds(0, 0, this.map.widthInPixels, this.map.heightInPixels);
      this.cameras.main.startFollow(player.group, true); //player anims

      this.anims.create({
        key: 'walk',
        frames: this.anims.generateFrameNumbers('player', {
          start: 2,
          end: 7
        }),
        frameRate: 10,
        repeat: -1
      });
      this.anims.create({
        key: 'idle',
        frames: [{
          key: 'player',
          frame: '0'
        }],
        frameRate: 10
      }); //debug graphics

      this.selector = this.add.graphics({
        lineStyle: {
          width: 1,
          color: 0xffffff,
          alpha: 1
        }
      });
      this.selector.strokeRect(0, 0, this.map.tileWidth, this.map.tileHeight);
      this.destRect = this.add.graphics({
        lineStyle: {
          width: 1,
          color: 0xffffff,
          alpha: 1
        }
      });
      this.destRect.strokeRect(0, 0, this.map.tileWidth, this.map.tileHeight); //pointer handling

      this.input.on('pointerdown', function (pointer) {
        var pointerTile = {
          x: this.map.worldToTileX(pointer.x + this.cameras.main.worldView.x),
          y: this.map.worldToTileY(pointer.y + this.cameras.main.worldView.y)
        };
        this.connection.invoke("move_request", {
          destination: pointerTile,
          playerId: this.playerId
        });
      }, this); //game tick handling

      this.connection.on("tick", function (state) {
        var map = state.maps.find(function (map) {
          return map.id === _this2.mapId;
        });
        _this2.actors = _this2.actors.filter(function (actor) {
          return map.actors.find(function (act) {
            return act.id === actor.id;
          });
        });
        var newActors = map.actors.filter(function (actor) {
          return !_this2.actors.find(function (act) {
            return act.id === actor.id;
          });
        }).map(function (actor) {
          return new _actor["default"](_this2, actor.id, actor.spriteId, actor.position);
        });
        _this2.actors = _this2.actors.concat(newActors);

        _this2.actors.forEach(function (actor) {
          return actor.update(map.actors.find(function (act) {
            return act.id === actor.id;
          }));
        });
      });
    }
  }, {
    key: "update",
    value: function update() {
      //draw the tile selector
      var worldPoint = this.input.activePointer.positionToCamera(this.cameras.main);
      var pointerTile = {
        x: this.map.worldToTileX(worldPoint.x),
        y: this.map.worldToTileY(worldPoint.y)
      };
      this.selector.x = this.map.tileToWorldX(pointerTile.x);
      this.selector.y = this.map.tileToWorldY(pointerTile.y); //draw destination DEBUG
      // let player = this.actors.find(actor => actor.id === this.playerId)
      // this.destRect.setPosition(this.map.tileToWorldX(player.currentAction?.nextStep.x), this.map.tileToWorldY(player.currentAction?.nextStep.y))
    }
  }]);

  return GameScene;
}(_phaser.Scene);

exports["default"] = GameScene;