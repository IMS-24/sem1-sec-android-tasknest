import { Body, Controller, Post, UnauthorizedException } from '@nestjs/common';
import { LoginService } from '../services/login.service';

@Controller('auth')
export class LoginController {
  constructor(private readonly loginService: LoginService) {}

  @Post('login')
  async login(@Body() loginDto: { email: string; password: string }) {
    const user = await this.loginService.validateUser(
      loginDto.email,
      loginDto.password,
    );
    if (!user) {
      throw new UnauthorizedException('Invalid credentials');
    }
    return {
      message: 'Login successful',
      user,
    };
  }
}
