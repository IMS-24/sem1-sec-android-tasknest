import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ShareTodo } from '../entities/share-todo.entity';
import { User } from '../entities/user.entity';
import { Todo } from '../entities/todo.entity';
import { ShareTodoController } from '../controllers/share-todo.controller';
import { ShareTodoService } from '../services/share-todo.service';

@Module({
  imports: [TypeOrmModule.forFeature([ShareTodo, User, Todo])],
  controllers: [ShareTodoController],
  providers: [ShareTodoService],
})
export class ShareTodoModule {}
