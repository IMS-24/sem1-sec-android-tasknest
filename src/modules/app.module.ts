import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ConfigModule, ConfigService } from '@nestjs/config';

import { LoginModule } from './login.module';
import { User } from '../entities/user.entity';
import { Todo } from '../entities/todo.entity';
import { UserTodo } from '../entities/user-todo.entity';
import { ShareTodo } from '../entities/share-todo.entity';
import { Attachment } from '../entities/attachment.entity';
import { TodoModule } from './todo.module';
import { UserTodoModule } from './user-todo.module';
import { ShareTodoModule } from './share-todo.module';
import { AttachmentModule } from './attachement.module';

@Module({
  imports: [
    ConfigModule.forRoot({
      isGlobal: true, // Makes the config available globally
      envFilePath: '.env', // Specifies the path to the .env file
    }),
    TypeOrmModule.forRootAsync({
      imports: [ConfigModule],
      inject: [ConfigService],
      useFactory: (configService: ConfigService) => ({
        type: 'postgres',
        host: configService.get<string>('POSTGRES_HOST'),
        port: configService.get<number>('POSTGRES_PORT'),
        username: configService.get<string>('POSTGRES_USER'),
        password: configService.get<string>('POSTGRES_PASSWORD'),
        database: configService.get<string>('POSTGRES_DB'),
        entities: [User, Todo, UserTodo, ShareTodo, Attachment],
        synchronize: true, // NOTE: Do not use `synchronize: true` in production
      }),
    }),
    LoginModule,
    TodoModule,
    UserTodoModule,
    ShareTodoModule,
    AttachmentModule,
  ],
})
export class AppModule {}
