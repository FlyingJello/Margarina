import Phaser from "phaser";

import GameScene from "./scenes/game";
import MenuScene from "./scenes/menu";

const config = {
  type: Phaser.AUTO,
  width: 480,
  height: 380,
  pixelArt: true,
  zoom: 2,
  parent: 'game',
  dom: {
    createContainer: true
  },
  scene: [MenuScene, GameScene]
};

window.addEventListener('load', () => {
  fetch("config.json")
  .then(response => response.json())
  .then(serverConfigs => {
    window.config = serverConfigs
    new Phaser.Game(config)
  })
  
})