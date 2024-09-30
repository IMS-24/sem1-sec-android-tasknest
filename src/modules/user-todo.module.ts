import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserTodo } from '../entities/user-todo.entity';
import { User } from '../entities/user.entity';
import { Todo } from '../entities/todo.entity';
import { UserTodoController } from '../controllers/user-todo.controller';
import { UserTodoService } from '../services/user-todo.service';

@Module({
  imports: [TypeOrmModule.forFeature([UserTodo, User, Todo])],
  controllers: [UserTodoController],
  providers: [UserTodoService],
})
export class UserTodoModule {}
