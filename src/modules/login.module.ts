import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { LoginService } from '../services/login.service';
import { LoginController } from '../controllers/login.controller';
import { User } from '../entities/user.entity';

@Module({
  imports: [TypeOrmModule.forFeature([User])],
  controllers: [LoginController],
  providers: [LoginService],
})
export class LoginModule {}
