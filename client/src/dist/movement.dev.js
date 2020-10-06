"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports["default"] = void 0;

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Movement =
/*#__PURE__*/
function () {
  function Movement(movement, player, physics, map) {
    _classCallCheck(this, Movement);

    this.sprite = sprite;
    this.id = id;
  }

  _createClass(Movement, [{
    key: "update",
    value: function update() {
      if (nextStep.x < this.currentPosition.x) {
        player.sprite.setFlip(true);
      }

      if (nextStep.x > this.currentPosition.x) {
        player.sprite.setFlip(false);
      }

      player.sprite.play('walk', true);
      var speed = player.speed * 0.02 * 16;
      speed = speed - speed * 0.1;
      var diagonalSpeed = Math.sqrt(Math.pow(speed, 2) * 2);
      physics.moveTo(player.sprite, map.tileToWorldX(this.nextStep.x), map.tileToWorldY(this.nextStep.y), speed);

      if (this.currentPosition.x !== this.nextStep.x && this.currentPosition.y !== this.nextStep.y) {
        physics.moveTo(player.sprite, map.tileToWorldX(this.nextStep.x), map.tileToWorldY(this.nextStep.y), diagonalSpeed);
      }
    }
  }]);

  return Movement;
}();

exports["default"] = Movement;