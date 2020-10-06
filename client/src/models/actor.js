export default class Actor {
  constructor(game, id, spriteId, position) {
    this.group = game.add.container(0.0) 
    this.sprite = game.add.sprite(0, 0, spriteId, 1).setOrigin(0, 0)
    this.group.add(this.sprite)
    game.physics.add.existing(this.group)

    this.id = id
    this.game = game
    this.position = position

    this.setPosition(position)
  }

  setPosition(pos) {
    this.group.setPosition(this.game.map.tileToWorldX(pos.x), this.game.map.tileToWorldY(pos.y))
  }

  update(actor) {
    this.currentAction = actor.currentAction
    this.speed = actor.speed
    this.position = actor.position

    let map = this.game.map
    let group = this.group
    let action = this.currentAction

    if (action == null || action.type === 0) {
      group.body.setVelocityX(0);
      group.body.setVelocityY(0);
      this.sprite.play('idle', true);

      this.setPosition(this.position)
    }
    else if (action.type === 1) {

      if (action.nextStep.x < action.position.x) {
        this.sprite.setFlip(true)
      }
      if (action.nextStep.x > action.position.x) {
        this.sprite.setFlip(false)
      }

      this.sprite.play('walk', true);

      this.group.setPosition(map.tileToWorldX(action.position.x), map.tileToWorldY(action.position.y))

      //interpolation stuff
      let speed = this.speed * 0.02 * 16
      speed = speed - speed * 0.25
      this.game.physics.moveTo(group, map.tileToWorldX(action.nextStep.x), map.tileToWorldY(action.nextStep.y), speed)
    }
  }

  destroy() {
    this.group.destroy()
  }
}