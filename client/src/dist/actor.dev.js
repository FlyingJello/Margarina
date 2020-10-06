"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Actor =
/*#__PURE__*/
function () {
  function Actor(game, id, spriteId, position) {
    _classCallCheck(this, Actor);

    this.group = game.add.container(0.0);
    this.sprite = game.add.sprite(position.x, position.y, spriteId, 1).setOrigin(0, 0);
    this.group.add(this.sprite);
    game.physics.add.existing(this.group);
    this.id = id;
    this.game = game;
    this.position = position;
    this.setPosition(position);
  }

  _createClass(Actor, [{
    key: "setPosition",
    value: function setPosition(pos) {
      this.group.setPosition(this.game.map.tileToWorldX(pos.x), this.game.map.tileToWorldY(pos.y));
    }
  }, {
    key: "update",
    value: function update(actor) {
      this.currentAction = actor.currentAction;
      this.speed = actor.speed;
      this.position = actor.position;
      var map = this.game.map;
      var group = this.group;
      var action = this.currentAction;

      if (action == null || action.type === 0) {
        group.body.setVelocityX(0);
        group.body.setVelocityY(0);
        this.sprite.play('idle', true);
        this.setPosition(this.position);
      } else if (action.type === 1) {
        if (action.nextStep.x < action.position.x) {
          this.sprite.setFlip(true);
        }

        if (action.nextStep.x > action.position.x) {
          this.sprite.setFlip(false);
        }

        this.sprite.play('walk', true);
        this.group.setPosition(map.tileToWorldX(action.position.x), map.tileToWorldY(action.position.y)); //interpolation stuff

        var speed = this.speed * 0.02 * 16;
        speed = speed - speed * 0.15;
        this.game.physics.moveTo(group, map.tileToWorldX(action.nextStep.x), map.tileToWorldY(action.nextStep.y), speed);
      }
    }
  }]);

  return Actor;
}();

exports["default"] = Actor;