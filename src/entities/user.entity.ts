import { Column, Entity, OneToMany, PrimaryGeneratedColumn } from 'typeorm';
import { UserTodo } from './user-todo.entity';
import { ShareTodo } from './share-todo.entity';

@Entity('users')
export class User {
  @PrimaryGeneratedColumn({ name: 'user_id' })
  id: number;

  @Column({ type: 'text', nullable: false })
  name: string;

  @Column({ type: 'text', unique: true, nullable: false })
  email: string;

  @Column({ type: 'text', nullable: false })
  password: string;

  @OneToMany(() => UserTodo, (userTodo) => userTodo.user)
  userTodos: UserTodo[];

  @OneToMany(() => ShareTodo, (shareTodo) => shareTodo.user)
  sharedTodos: ShareTodo[];
}
