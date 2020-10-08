import { Scene } from 'phaser'

import font from '../assets/font/font1.png';
import fontData from '../assets/font/font1.xml';

export default class LoadingScene extends Scene {
  constructor() {
    super({ key: 'LoadingScene' })
  }

  preload() {
    this.load.bitmapFont('font', font, fontData);
  }

  create() {
    let loadingText = this.add.bitmapText(this.game.canvas.width / 2 - 24, this.game.canvas.height / 2 - 80, 'font', "", 64)

    this.tweens.addCounter({
      from: 0,
      to: 4,
      duration: 500,
      repeat: -1,
      onUpdate: function (tween) {
        loadingText.text = ".".repeat(Math.min(tween.getValue(), 3))
      }
    });
  }

  update() { }

}